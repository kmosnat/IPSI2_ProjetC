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
	this->nbDataImg = nbChamps;
	this->dataFromImg.resize(nbChamps);

	this->imgPt = new CImageCouleur(nbLig, nbCol);
	CImageCouleur out(nbLig, nbCol);

	// on remplit les pixels de source
	byte* pixPtr = (byte*)data;

	for (int y = 0; y < nbLig; y++)
	{
		for (int x = 0; x < nbCol; x++)
		{
			this->imgPt->operator()(y, x)[0] = pixPtr[3 * x + 2];
			this->imgPt->operator()(y, x)[1] = pixPtr[3 * x + 1];
			this->imgPt->operator()(y, x)[2] = pixPtr[3 * x];
		}
		pixPtr += stride; // largeur une seule ligne gestion multiple 32 bits
	}
}

ClibIHM::ClibIHM(int nbChamps, byte* data, byte* dataGT, int stride, int nbLig, int nbCol){
	this->nbDataImg = nbChamps;
	this->dataFromImg.resize(nbChamps);

	CImageCouleur out(nbLig, nbCol);

	ClibIHM GT;
	GT.imgPt = new CImageCouleur(nbLig, nbCol);

	// on remplit les pixels GT
	byte* pixGTPtr = (byte*)dataGT;

	for (int y = 0; y < nbLig; y++)
	{
		for (int x = 0; x < nbCol; x++)
		{
			GT.imgPt->operator()(y, x)[0] = pixGTPtr[3 * x + 2];
			GT.imgPt->operator()(y, x)[1] = pixGTPtr[3 * x + 1];
			GT.imgPt->operator()(y, x)[2] = pixGTPtr[3 * x];
		}
		pixGTPtr += stride; // largeur une seule ligne gestion multiple 32 bits
	}

	
	this->imgPt = new CImageCouleur(nbLig, nbCol);
	

	// on remplit les pixels de source

	byte* pixPtr = (byte*)data;

	for (int y = 0; y < nbLig; y++)
	{
		for (int x = 0; x < nbCol; x++)
		{
			this->imgPt->operator()(y, x)[0] = pixPtr[3 * x + 2];
			this->imgPt->operator()(y, x)[1] = pixPtr[3 * x + 1];
			this->imgPt->operator()(y, x)[2] = pixPtr[3 * x ];
		}
		pixPtr += stride; // largeur une seule ligne gestion multiple 32 bits
	}

	CImageNdg img;
	CImageNdg imgGT;

	img = this->imgPt->plan().morphologie("dilatation", "disk", 3).ouverture("disk", 3);

	int seuilBas = 128;
	int seuilHaut = 255;
	imgGT = GT.imgPt->plan().seuillage("otsu", seuilBas, seuilHaut);

	// Application des opérations Top-Hat
	CImageNdg blackTopHat = img.blackTopHat("disk", 17);
	CImageNdg whiteTopHat = img.whiteTopHat("disk", 17);

	// Calcul des corrélations pour déterminer quelle image top-hat utiliser
	double white_cor = whiteTopHat.correlation(imgGT);
	double black_cor = blackTopHat.correlation(imgGT);

	CImageNdg img_tophat = (white_cor > black_cor) ? whiteTopHat : blackTopHat;

	CImageNdg seuil = img_tophat.seuillage("otsu", seuilBas, seuilHaut);

	double iou = floor((seuil.indicateurPerformance(imgGT, "iou") * 100) * 100) / 100;;

	this->dataFromImg.at(0) = iou;

	for (int i = 0; i < seuil.lireNbPixels(); i++)
	{
		out(i)[0] = (unsigned char)(255*(int)seuil(i));
		out(i)[1] = 0;
		out(i)[2] = 0;
	}
		
	pixPtr = (byte*)data;
	for (int y = 0; y < nbLig; y++)
	{
		for (int x = 0; x < nbCol; x++)
		{
			pixPtr[3 * x + 2] = out(y, x)[0];
			pixPtr[3 * x + 1] = out(y, x)[1];
			pixPtr[3 * x] = out(y, x)[2];
		}
		pixPtr += stride; // largeur une seule ligne gestion multiple 32 bits
	}
}

ClibIHM::~ClibIHM() {
	
	if (imgPt)
		(*this->imgPt).~CImageCouleur(); 
	this->dataFromImg.clear();
}