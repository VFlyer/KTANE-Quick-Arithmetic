using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
public class QuickArithmetic : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable Submit;
    public KMSelectable[] Arrows; //BL BR TL TR
    public TextMesh Mainfatass;
    public TextMesh Leftasscheek;
    public TextMesh Rightasscheek;

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved = false;

    int Leftasshole = 0; //eXish if you are reading this, do not fuckign change the variable names
    int Rightasshole = 0;
    int Thiccnumerouno = -1;
    int Thiccnumerodos = -1;
    int FinalPenileLength = -1;
    string SN = "FATASSANSWERCONFIRMCONFIRMANSWERGUCCI";
    int Callitsomethingyoucanremember = -1;

    void Awake () {
        moduleId = moduleIdCounter++;

        foreach (KMSelectable Arrow in Arrows) {
            KMSelectable pressedArrow = Arrow;
            Arrow.OnInteract += delegate () { ArrowPress(pressedArrow); return false; };
        }

        Submit.OnInteract += delegate () { PressSugma(); return false; };

    }

  void Start () {
    Thiccnumerouno = UnityEngine.Random.Range(0,100000000); //Make sure preceeding bullshit happendefn aldfnaksdjf
    Thiccnumerodos = UnityEngine.Random.Range(0,100000000);
    Debug.LogFormat("[Quick Arithmetic #{0}] The two numbers are {1} and {2}.", moduleId, Thiccnumerouno, Thiccnumerodos);
    FinalPenileLength = Math.Abs(Thiccnumerouno - Thiccnumerodos);
    SN = Bomb.GetSerialNumber();
    Callitsomethingyoucanremember = (int)Char.GetNumericValue(SN[5]);
    FinalPenileLength *= Callitsomethingyoucanremember;
    FinalPenileLength += Bomb.GetBatteryCount();
    FinalPenileLength %= 100;
    Debug.LogFormat("[Quick Arithmetic #{0}] The final number is {1}.", moduleId, FinalPenileLength);
    StartCoroutine(Ibecyclingthesebitches());
  }

  void PressSugma () {
    Submit.AddInteractionPunch();
    GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
    if (Leftasshole * 10 + Rightasshole == FinalPenileLength) {
      if (moduleSolved == false) {
        Debug.LogFormat("[Quick Arithmetic #{0}] The number you have submitted is {1}. Module disarmed.", moduleId, FinalPenileLength);
      }
      GetComponent<KMBombModule>().HandlePass();
      StopAllCoroutines();
      moduleSolved = true;
      if (FinalPenileLength == 69) {
        Mainfatass.text = ";)";
      }
      else {
        Mainfatass.text = " ";
      }
    }
    else {
      if (moduleSolved == false) {
        GetComponent<KMBombModule>().HandleStrike();
        Debug.LogFormat("[Quick Arithmetic #{0}] The number you have submitted is {1}. Strike, dipass.", moduleId, Leftasshole * 10 + Rightasshole);
      }
    }
  }

  void ArrowPress (KMSelectable Arrow) {
    Arrow.AddInteractionPunch();
    GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
    if (Arrow == Arrows[0]) {
      Leftasshole = (Leftasshole + 9) % 10;
      Leftasscheek.text = Leftasshole.ToString();
    }
    else if (Arrow == Arrows[1]) {
      Rightasshole = (Rightasshole + 9) % 10;
      Rightasscheek.text = Rightasshole.ToString();
    }
    else if (Arrow == Arrows[2]) {
      Leftasshole = (Leftasshole + 1) % 10;
      Leftasscheek.text = Leftasshole.ToString();
    }
    else if (Arrow == Arrows[3]) {
      Rightasshole = (Rightasshole + 1) % 10;
      Rightasscheek.text = Rightasshole.ToString();
    }
    else {
      Debug.Log("Fuck");
    }
  }

  IEnumerator Ibecyclingthesebitches() {
    Mainfatass.text = ((Thiccnumerouno % 100000000 - Thiccnumerouno % 10000000) / 10000000).ToString();
    yield return new WaitForSeconds(0.25f);
	Mainfatass.text = " ";
	yield return new WaitForSeconds(0.25f);
    Mainfatass.text = ((Thiccnumerouno % 10000000 - Thiccnumerouno % 1000000) / 1000000).ToString();
    yield return new WaitForSeconds(0.25f);
	Mainfatass.text = " ";
	yield return new WaitForSeconds(0.25f);
    Mainfatass.text = ((Thiccnumerouno % 1000000 - Thiccnumerouno % 100000) / 100000).ToString();
    yield return new WaitForSeconds(0.25f);
	Mainfatass.text = " ";
	yield return new WaitForSeconds(0.25f);
    Mainfatass.text = ((Thiccnumerouno % 100000 - Thiccnumerouno % 10000) / 10000).ToString();
    yield return new WaitForSeconds(0.25f);
	Mainfatass.text = " ";
	yield return new WaitForSeconds(0.25f);
    Mainfatass.text = ((Thiccnumerouno % 10000 - Thiccnumerouno % 1000) / 1000).ToString();
    yield return new WaitForSeconds(0.25f);
	Mainfatass.text = " ";
	yield return new WaitForSeconds(0.25f);
    Mainfatass.text = ((Thiccnumerouno % 1000 - Thiccnumerouno % 100) / 100).ToString();
    yield return new WaitForSeconds(0.25f);
	Mainfatass.text = " ";
	yield return new WaitForSeconds(0.25f);
    Mainfatass.text = ((Thiccnumerouno % 100 - Thiccnumerouno % 10) / 10).ToString();
    yield return new WaitForSeconds(0.25f);
	Mainfatass.text = " ";
	yield return new WaitForSeconds(0.25f);
    Mainfatass.text = (Thiccnumerouno % 10).ToString();
    yield return new WaitForSeconds(0.25f);
    Mainfatass.text = " ";
    yield return new WaitForSeconds(1);
    Mainfatass.text = ((Thiccnumerodos % 100000000 - Thiccnumerodos % 10000000) / 10000000).ToString();
    yield return new WaitForSeconds(0.25f);
	Mainfatass.text = " ";
	yield return new WaitForSeconds(0.25f);
    Mainfatass.text = ((Thiccnumerodos % 10000000 - Thiccnumerodos % 1000000) / 1000000).ToString();
    yield return new WaitForSeconds(0.25f);
	Mainfatass.text = " ";
	yield return new WaitForSeconds(0.25f);
    Mainfatass.text = ((Thiccnumerodos % 1000000 - Thiccnumerodos % 100000) / 100000).ToString();
    yield return new WaitForSeconds(0.25f);
	Mainfatass.text = " ";
	yield return new WaitForSeconds(0.25f);
    Mainfatass.text = ((Thiccnumerodos % 100000 - Thiccnumerodos % 10000) / 10000).ToString();
    yield return new WaitForSeconds(0.25f);
	Mainfatass.text = " ";
	yield return new WaitForSeconds(0.25f);
    Mainfatass.text = ((Thiccnumerodos % 10000 - Thiccnumerodos % 1000) / 1000).ToString();
    yield return new WaitForSeconds(0.25f);
	Mainfatass.text = " ";
	yield return new WaitForSeconds(0.25f);
    Mainfatass.text = ((Thiccnumerodos % 1000 - Thiccnumerodos % 100) / 100).ToString();
    yield return new WaitForSeconds(0.25f);
	Mainfatass.text = " ";
	yield return new WaitForSeconds(0.25f);
    Mainfatass.text = ((Thiccnumerodos % 100 - Thiccnumerodos % 10) / 10).ToString();
    yield return new WaitForSeconds(0.25f);
	Mainfatass.text = " ";
	yield return new WaitForSeconds(0.25f);
    Mainfatass.text = (Thiccnumerodos % 10).ToString();
    yield return new WaitForSeconds(0.25f);
    Mainfatass.text = " ";
    yield return new WaitForSeconds(1);
    StartCoroutine(Ibecyclingthesebitches());
  }
}
