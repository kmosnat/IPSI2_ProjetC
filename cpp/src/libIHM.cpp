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
	imgNdgPt = new CImageNdg(nbLig, nbCol);
	if (!imgPt) {
		throw std::runtime_error("Erreur allocation CImageCouleur");
	}

	byte* pixPtr = this->data;

	for (int y = 0; y < nbLig; y++)
	{
		for (int x = 0; x < nbCol; x++)
		{
			// R�cup�ration des valeurs RGB
			imgPt->operator()(y, x)[0] = pixPtr[3 * x + 2]; // Bleu
			imgPt->operator()(y, x)[1] = pixPtr[3 * x + 1]; // Vert
			imgPt->operator()(y, x)[2] = pixPtr[3 * x];     // Rouge

			// Conversion en niveau de gris
			imgNdgPt->operator()(y, x) = (int)(0.299 * pixPtr[3 * x] + 0.587 * pixPtr[3 * x + 1] + 0.114 * pixPtr[3 * x + 2]);
		}
		pixPtr += stride;
	}

	if (!imgNdgPt) {
		delete imgPt;
		throw std::runtime_error("Erreur allocation CImageNdg");
	}
}

CImageNdg ClibIHM::toBinaire()
{
	CImageNdg imgNdg(NbLig, NbCol);
	for (int y = 0; y < NbLig; y++)
	{
		for (int x = 0; x < NbCol; x++)
		{
			if (this->imgNdgPt->operator()(y, x) == 255)
			{
				imgNdg(y, x) = 1;
			}
			else
			{
				imgNdg(y, x) = 0;
			}
		}
	}
	return imgNdg;
}

void ClibIHM::writeBinaryImage(CImageNdg data)
{
	for (int y = 0; y < NbLig; y++)
	{
		for (int x = 0; x < NbCol; x++)
		{
			if (data(y, x) == 1)
			{
				this->imgNdgPt->operator()(y, x) = 255;
			}
			else
			{
				this->imgNdgPt->operator()(y, x) = 0;
			}
		}
	}
}

void ClibIHM::writeImage(CImageNdg img)
{
	for (int y = 0; y < NbLig; y++)
	{
		for (int x = 0; x < NbCol; x++)
		{
			this->imgNdgPt->operator()(y, x) = img(y, x);
		}
	}
}

void ClibIHM::filter(std::string methode, int kernel)
{
	if (methode == "moyen")
	{
		this->writeImage(this->imgNdgPt->filtrage("moyennage", kernel, kernel));
	}
	else if (methode == "median")
	{
		this->writeImage(this->imgNdgPt->filtrage("median", kernel, kernel));
	}

	this->persitData(this->imgNdgPt, COULEUR::RVB);
}

void ClibIHM::runProcess(ClibIHM* pImgGt)
{
	int seuilBas = 0;
	int seuilHaut = 255;

	CImageNdg inv_whiteTopHat, whiteTopHat;

	// Cr�ation et d�marrage des threads pour calculer whiteTopHat et inv_whiteTopHat
	std::thread th1([&] {
		inv_whiteTopHat = this->imgNdgPt->transformation().whiteTopHat("disk", 17);
		});
	std::thread th2([&] {
		whiteTopHat = this->imgNdgPt->whiteTopHat("disk", 17);
		});

	// Attendez que th1 et th2 terminent
	th1.join();
	th2.join();

	CImageNdg inv_seuil, seuil;

	// Utilisation des r�sultats dans de nouveaux threads
	std::thread th3([&] {
		inv_seuil = inv_whiteTopHat.seuillage("otsu", seuilBas, seuilHaut).morphologie("erosion", "V8", 9).morphologie("dilatation", "V8", 9);
		});
	std::thread th4([&] {
		seuil = whiteTopHat.seuillage("otsu", seuilBas, seuilHaut).morphologie("erosion", "V8", 9).morphologie("dilatation", "V8", 9);
		});

	// Attendez que th3 et th4 terminent
	th3.join();
	th4.join();

	CImageNdg GT = pImgGt->toBinaire();

	if (inv_seuil.correlation(GT) > seuil.correlation(GT))
	{
		this->writeBinaryImage(inv_seuil);
	}
	else
	{
		this->writeBinaryImage(seuil);
	}

	this->score(pImgGt);
	this->compare(pImgGt);

	this->persitData(this->imgNdgPt, COULEUR::RVB);
}


void ClibIHM::compare(ClibIHM* pImgGt)
{
	CImageCouleur out(NbLig, NbCol);

	for (int y = 0; y < NbLig; y++)
	{
		for (int x = 0; x < NbCol; x++)
		{
			if (this->imgNdgPt->operator()(y, x) == pImgGt->imgNdgPt->operator()(y, x))
			{
				out(y, x)[0] = 0;
				out(y, x)[1] = 255;
				out(y, x)[2] = 0;
			}
			else
			{
				out(y, x)[0] = 255;
				out(y, x)[1] = 0;
				out(y, x)[2] = 0;
			}
			if (this->imgNdgPt->operator()(y, x) == 0 && pImgGt->imgNdgPt->operator()(y, x) == 0)
			{
				out(y, x)[0] = 0;
				out(y, x)[1] = 0;
				out(y, x)[2] = 0;

			}
		}
	}

	// Ecriture de l'image
	byte* pixPtr = pImgGt->data;
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

void ClibIHM::score(ClibIHM* pImgGt)
{
	// Score IOU
	CImageNdg img = this->toBinaire();
	CImageNdg GT = pImgGt->toBinaire();

	int intersection = 0;
	int union_ = 0;

	for (int y = 0; y < NbLig; y++)
	{
		for (int x = 0; x < NbCol; x++)
		{
			if (img(y, x) == 1 && GT(y, x) == 1)
			{
				intersection++;
			}
			if (img(y, x) == 1 || GT(y, x) == 1)
			{
				union_++;
			}
		}
	}
	this->dataFromImg.at(0) = floor(((double)intersection / (double)union_) * 10000) / 100;

	// Score de Vinet
	std::vector<SIGNATURE_Forme> st, sr;

	CImageClasse lab(img, "V8");
	CImageClasse labGT(GT, "V8");

	st = lab.signatures();
	sr = labGT.signatures();

	int nt = st.size(), nr = sr.size();
	int nm = min(nt, nr);
	float totalArea = 0.0f, score = 0.0f;

	for (int i = 0; i < nm; i++) {
		SIGNATURE_Forme* bestchoice = &sr[0];
		float minDis = distanceSQ(st[i], *bestchoice);

		for (int j = 1; j < nr; j++) {
			float currentDis = distanceSQ(st[i], sr[j]);
			if (currentDis < minDis && sr[j].surface == st[i].surface) {
				bestchoice = &sr[j];
				minDis = currentDis;
			}
		}

		SIGNATURE_Forme compareRegion = *bestchoice;
		totalArea += st[i].rectEnglob_Hi * st[i].rectEnglob_Hj;
		score += distanceSQ(st[i], compareRegion);
	}

	for (int i = nm; i < max(nt, nr); i++) {
		if (i < nr) {
			totalArea += sr[i].rectEnglob_Hi * sr[i].rectEnglob_Hj;
		}
		if (i < nt) {
			totalArea += st[i].rectEnglob_Hi * st[i].rectEnglob_Hj;
		}
	}

	this->dataFromImg.at(1) = floor((score / totalArea * 10000) / 100);
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




