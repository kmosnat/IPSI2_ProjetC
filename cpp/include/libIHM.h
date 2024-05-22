#pragma once

// biblioth�ques utilis�e en traitement et analyse
#include "ImageClasse.h"
#include "ImageNdg.h"
#include "ImageCouleur.h"
#include "ImageDouble.h"

#include <windows.h>
#include <thread>
#include <vector>

enum class COULEUR
{
	RVB,
	rouge,
	vert,
	bleu
};

float distanceSQ(SIGNATURE_Forme p1, SIGNATURE_Forme p2) { // conna�tre la distance entre 2 centres de gravit� d'ensemble connexe
	return (p1.centreGravite_i - p2.centreGravite_i) * (p1.centreGravite_i - p2.centreGravite_i) + (p1.centreGravite_j - p2.centreGravite_j) * (p1.centreGravite_j - p2.centreGravite_j);
}

float localIoU(CImageNdg* img, CImageNdg* GT, SIGNATURE_Forme region) { // comparaison entre les 2 images : trait� (img) et v�rit� terrain (GT) de chaque ensemble connexe de l'image
	// on r�cup�re les coordonn�es du rectangle englobant de l'ensemble connexe (rectangle englobant = plus petit rectangle qui peut contenir tout un 
	// ensemble connexe de l'image
	int x1 = region.rectEnglob_Hi;	//colonne min
	int y1 = region.rectEnglob_Hj;	//ligne min
	int x2 = region.rectEnglob_Bi;	//colonne max
	int y2 = region.rectEnglob_Bj;	//ligne max

	// on r�cup�re les coordonn�es du rectangle englobant de la v�rit� terrain
	int x1GT = region.rectEnglob_Hi;
	int y1GT = region.rectEnglob_Hj;
	int x2GT = region.rectEnglob_Bi;
	int y2GT = region.rectEnglob_Bj;

	// on cherche le rectangle min qui englobe les 2 ensembles connexes
	int x1Inter = max(x1, x1GT);
	int y1Inter = max(y1, y1GT);
	int x2Inter = min(x2, x2GT);
	int y2Inter = min(y2, y2GT);

	int wInter = max(0, x2Inter - x1Inter);
	int hInter = max(0, y2Inter - y1Inter);

	int wUnion = max(0, x2 - x1) + max(0, x2GT - x1GT) - wInter;
	int hUnion = max(0, y2 - y1) + max(0, y2GT - y1GT) - hInter;

	return (float)(wInter * hInter) / (wUnion * hUnion);
}

class ClibIHM {

	///////////////////////////////////////
private:
	///////////////////////////////////////

	// data n�cessaires � l'IHM donc fonction de l'application cibl�e
	int						nbDataImg; // nb champs Texte de l'IHM
	std::vector<double>		dataFromImg; // champs Texte de l'IHM
	CImageCouleur* imgPt;       // pour utiliser les m�thodes de la classe CImageCouleur
	CImageNdg* imgNdgPt;     // pour utiliser les m�thodes de la classe CImageNdg
	byte* data;       // champs Texte de l'IHM
	int NbLig;
	int NbCol;
	int stride;

	///////////////////////////////////////
public:
	///////////////////////////////////////

	// constructeurs
	_declspec(dllexport) ClibIHM(); // par d�faut

	_declspec(dllexport) ClibIHM(int nbChamps, byte* data, int stride, int nbLig, int nbCol); // pour image format bmp C#

	_declspec(dllexport) ~ClibIHM(); //destruceteur

	// get et set 

	_declspec(dllexport) int lireNbChamps() const {
		return nbDataImg;
	}

	_declspec(dllexport) double lireChamp(int i) const {
		return dataFromImg.at(i);
	}

	_declspec(dllexport) CImageCouleur* imgData() const {
		return imgPt;
	}

	_declspec(dllexport) void ecrireChamp(int i, double val) {
		dataFromImg.at(i) = val;
	}

	_declspec(dllexport) CImageNdg toBinaire(); //mettre le champ binaire � 1, n�cessaire pour faire du filtrage
	_declspec(dllexport) void writeBinaryImage(CImageNdg img);

	_declspec(dllexport) void writeImage(CImageNdg img); // op�rateur de copie de l'image d'entr�e
	_declspec(dllexport) void filter(std::string methode,int kernel, char* str);
	_declspec(dllexport) void runProcess(ClibIHM* pImgGt);

	_declspec(dllexport) void compare(ClibIHM* pImgGt);
	_declspec(dllexport) void score(ClibIHM* pImgGt);

	_declspec(dllexport) void persitData(CImageNdg* pImg, COULEUR couleur);
};

extern "C" _declspec(dllexport) ClibIHM* objetLib()
{
	ClibIHM* pImg = new ClibIHM();
	return pImg;
}

extern "C" _declspec(dllexport) ClibIHM* objetLibDataImg(int nbChamps, byte* data, int stride, int nbLig, int nbCol)
{
	ClibIHM* pImg = new ClibIHM(nbChamps, data, stride, nbLig, nbCol);
	return pImg;
}

extern "C" _declspec(dllexport) ClibIHM * meanFilter(ClibIHM* pImg, int kernel, char* str)
{
	pImg->filter("moyen", kernel, str);
	return pImg;
}

extern "C" _declspec(dllexport) ClibIHM * medianFilter(ClibIHM* pImg, int kernel, char* str)
{
	pImg->filter("median", kernel, str);
	return pImg;
}

extern "C" _declspec(dllexport) ClibIHM* process(ClibIHM* pImg, ClibIHM* pImgGt)
{
	pImg->runProcess(pImgGt);
	return pImgGt;
}

extern "C" _declspec(dllexport) double valeurChamp(ClibIHM* pImg, int i)
{
	return pImg->lireChamp(i);
}