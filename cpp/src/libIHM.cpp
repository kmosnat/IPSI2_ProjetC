#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#include <windows.h>
#include <cmath>
#include <vector>
#include <ctime>
#include <stack>

#include "libIHM.h"

ClibIHM::ClibIHM() {

	this->nbDataImg = 0;
	this->dataFromImg.clear();
	this->imgPt = NULL;
}

ClibIHM::ClibIHM(int nbChamps, byte* data, int stride, int nbLig, int nbCol)
{
	if (data == nullptr) {
		throw std::invalid_argument("Aucune Data");
	}

	nbDataImg = nbChamps;
	dataFromImg.resize(nbChamps);
	this->data = data;
	this->NbLig = nbLig;
	this->NbCol = nbCol;
	this->stride = stride;

	imgPt = new CImageCouleur(nbLig, nbCol);
	if (!imgPt) {
		throw std::runtime_error("Erreur allocation CImageCouleur");
	}

	byte* pixPtr = this->data;

	for (int y = 0; y < nbLig; y++)
	{
		for (int x = 0; x < nbCol; x++)
		{
			// Récupération des valeurs RGB
			imgPt->operator()(y, x)[0] = pixPtr[3 * x + 2]; // Bleu
			imgPt->operator()(y, x)[1] = pixPtr[3 * x + 1]; // Vert
			imgPt->operator()(y, x)[2] = pixPtr[3 * x];     // Rouge
		}
		pixPtr += stride;
	}

	imgNdgPt = new CImageNdg(imgPt->plan());
	if (!imgNdgPt) {
		delete imgPt;
		throw std::runtime_error("Erreur allocation CImageNdg");
	}
}

void ClibIHM::runProcess(ClibIHM* pImgGt)
{
	
	int seuilBas = 128;
	int seuilHaut = 255;

	// Application des opérations Top-Hat
	CImageNdg whiteTopHat = this->imgNdgPt->whiteTopHat("disk", 17);

	CImageNdg seuil = whiteTopHat.seuillage("otsu", seuilBas, seuilHaut).morphologie("erosion", "V8", 9).morphologie("dilatation", "V8", 9);
	this->persitData(seuil);

	CImageNdg seuilGT = pImgGt->imgNdgPt->seuillage("otsu", seuilBas, seuilHaut);
	pImgGt->persitData(seuilGT);

	//this->dataFromImg.at(0) = floor((seuil.indicateurPerformance(imgGT, "iou") * 100) * 100) / 100;
}

void ClibIHM::persitData(CImageNdg pImg)
{
	CImageCouleur out(NbLig, NbCol);

	
	// Conversion de l'image en couleur
	for (int i = 0; i < pImg.lireNbPixels(); i++)
	{
		out(i)[0] = (unsigned char)(255 * (int)pImg(i));
		out(i)[1] = 0;
		out(i)[2] = 0;
	}

	// Ecriture de l'image
	byte* pixPtr = this->data;
	for (int y = 0; y < NbLig; y++)
	{
		for (int x = 0; x < NbCol; x++)
		{
			pixPtr[3 * x + 2] = out(y, x)[0]; // Bleu
			pixPtr[3 * x + 1] = out(y, x)[1]; // Vert
			pixPtr[3 * x] = out(y, x)[2];	 // Rouge
		}
		pixPtr += stride;
	}
}


ClibIHM::~ClibIHM() {
	
	if (imgPt)
		(*this->imgPt).~CImageCouleur(); 
	this->dataFromImg.clear();
}




