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

ClibIHM::ClibIHM(int nbChamps, byte* data, int stride, int nbLig, int nbCol) {
	nbDataImg = nbChamps;
	dataFromImg.resize(nbChamps);

	this->data = data;
	this->NbLig = nbLig;
	this->NbCol = nbCol;
	this->stride = stride;

	imgPt = new CImageCouleur(nbLig, nbCol);
	CImageCouleur out(nbLig, nbCol);

	// on remplit les pixels de source
	byte* pixPtr = this->data;

	for (int y = 0; y < nbLig; y++)
	{
		for (int x = 0; x < nbCol; x++)
		{
			imgPt->operator()(y, x)[0] = pixPtr[3 * x + 2];
			imgPt->operator()(y, x)[1] = pixPtr[3 * x + 1];
			imgPt->operator()(y, x)[2] = pixPtr[3 * x];
		}
		pixPtr += stride; // largeur une seule ligne gestion multiple 32 bits
	}
	imgNdgPt = this->imgPt->plan();
}

void ClibIHM::runProcess(ClibIHM* pImgGt)
{
	CImageNdg img;
	CImageCouleur out(NbLig, NbCol);

	img = this->imgNdgPt;

	int seuilBas = 128;
	int seuilHaut = 255;

	// Application des opérations Top-Hat
	CImageNdg whiteTopHat = img.whiteTopHat("disk", 17);

	CImageNdg seuil = whiteTopHat.seuillage("otsu", seuilBas, seuilHaut).morphologie("erosion", "V8", 9).morphologie("dilatation", "V8", 9);

	for (int i = 0; i < seuil.lireNbPixels(); i++)
	{
		out(i)[0] = (unsigned char)(255 * (int)seuil(i));
		out(i)[1] = 0;
		out(i)[2] = 0;
	}

	byte* pixPtr = this->data;
	for (int y = 0; y < NbLig; y++)
	{
		for (int x = 0; x < NbCol; x++)
		{
			pixPtr[3 * x + 2] = out(y, x)[0];
			pixPtr[3 * x + 1] = out(y, x)[1];
			pixPtr[3 * x] = out(y, x)[2];
		}
		pixPtr += stride; // largeur une seule ligne gestion multiple 32 bits
	}
	this->dataFromImg.at(0) = floor((this->imgPt->plan().indicateurPerformance(pImgGt->imgPt->plan(), "iou") * 100) * 100) / 100;
}


ClibIHM::~ClibIHM() {
	
	if (imgPt)
		(*this->imgPt).~CImageCouleur(); 
	this->dataFromImg.clear();
}




