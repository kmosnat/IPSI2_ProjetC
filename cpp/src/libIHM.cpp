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

	CImageNdg whiteTopHat = this->imgNdgPt->whiteTopHat("disk", 3);

	CImageNdg seuil = whiteTopHat.seuillage("otsu", seuilBas, seuilHaut).morphologie("erosion", "V8", 3).morphologie("dilatation", "V8", 3);

	for (int y = 0; y < NbLig; y++)
	{
		for (int x = 0; x < NbCol; x++)
		{
			if (seuil(y, x) == 1)
			{
				this->imgNdgPt->operator()(y, x) = 255;
			}
			else
			{
				this->imgNdgPt->operator()(y, x) = 0;
			}
		}
	}

	pImgGt->persitData(pImgGt->imgNdgPt, COULEUR::vert);
	this->persitData(this->imgNdgPt, COULEUR::rouge);
}


void ClibIHM::persitData(CImageNdg* pImg, COULEUR color)
{
	CImageCouleur out(NbLig, NbCol);

	// Conversion de l'image en couleur
	for (int y = 0; y < NbLig; y++)
	{
		for (int x = 0; x < NbCol; x++)
		{
			if (color == COULEUR::RVB)
			{
				out(y, x)[0] = pImg->operator()(y, x);
				out(y, x)[1] = pImg->operator()(y, x);
				out(y, x)[2] = pImg->operator()(y, x);
			}
			else if (color == COULEUR::rouge)
			{
				out(y, x)[0] = pImg->operator()(y, x);
				out(y, x)[1] = 0;
				out(y, x)[2] = 0;
			}
			else if (color == COULEUR::vert)
			{
				out(y, x)[0] = 0;
				out(y, x)[1] = pImg->operator()(y, x);
				out(y, x)[2] = 0;
			}
			else if (color == COULEUR::bleu)
			{
				out(y, x)[0] = 0;
				out(y, x)[1] = 0;
				out(y, x)[2] = pImg->operator()(y, x);
			}
		}
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




