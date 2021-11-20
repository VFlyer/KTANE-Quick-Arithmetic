using System;
using System.Collections;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class QuickArithmetic : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMAudio Audio;

   public TextMesh[] Displays;
   public TextMesh[] InputDisplays;
   public KMSelectable[] Arrows;
   public KMSelectable Submit;

   public GameObject[] LEDs;
   public Material Lit;

   Coroutine LDisplaySequences;
   Coroutine RDisplaySequences;

   int[] LeftSequenceN = new int[8];
   int[] RightSequence = new int[8];
   int[] ColorSequence = new int[8];
   byte[][] Colors = new byte[][] {
      new byte[] {255, 0, 0},
      new byte[] {0, 0, 255},
      new byte[] {0, 255, 0},
      new byte[] {255, 255, 0},
      new byte[] {255, 255, 255},
      new byte[] {64, 64, 64},
      new byte[] {255, 128, 0},
      new byte[] {255, 192, 203},
      new byte[] {160, 32, 240},
      new byte[] {0, 255, 255},
      new byte[] {150, 75, 0},
   };
   int[] TempPrimary = new int[8];
   int[] TempSecondary = new int[8];
   int[] Answers = new int[8];
   static readonly int[] Table = { 69, 43, 94, 86, 12, 53, 87, 65, 8, 67, 89, 42, 35, 76, 18, 74, 56, 43, 29, 1, 0, 46, 57, 80, 18, 49, 81, 43};
   int SubmissionIndex;
   int FinalColor;
   byte Opacity = 255;

   Vector3 InitialLocation = new Vector3();

   static int ModuleIdCounter = 1;
   int ModuleId;
   private bool ModuleSolved;

   void Awake () {
      ModuleId = ModuleIdCounter++;

      foreach (KMSelectable Arrow in Arrows) {
         Arrow.OnInteract += delegate () { ArrowPress(Arrow); return false; };
      }

      Submit.OnInteract += delegate () { SubmitPress(); return false; };

   }

   #region Button

   void SubmitPress () {
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Submit.transform);
      Submit.AddInteractionPunch();
      if (ModuleSolved) {
         return;
      }
      if (int.Parse(InputDisplays[0].text) * 10 + int.Parse(InputDisplays[1].text) == Answers[SubmissionIndex]) {
         LEDs[SubmissionIndex].GetComponent<MeshRenderer>().material = Lit;
         SubmissionIndex++;
         if (SubmissionIndex == 8) {
            GetComponent<KMBombModule>().HandlePass();
            Audio.PlaySoundAtTransform("Power Out", transform);
            ModuleSolved = true;
            StopCoroutine(LDisplaySequences);
            StopCoroutine(RDisplaySequences);
            StartCoroutine(Fade());
         }
      }
      else {
         GetComponent<KMBombModule>().HandleStrike();
      }
   }

   void ArrowPress (KMSelectable Arrow) {
      Audio.PlaySoundAtTransform("Selection", Arrow.transform);
      if (Arrow == Arrows[0]) {
         InputDisplays[0].text = ((int.Parse(InputDisplays[0].text) + 1) % 10).ToString();
      }
      else if (Arrow == Arrows[1]) {
         InputDisplays[1].text = ((int.Parse(InputDisplays[1].text) + 1) % 10).ToString();
      }
      else if (Arrow == Arrows[2]) {
         InputDisplays[0].text = ((int.Parse(InputDisplays[0].text) - 1) < 0 ? 9 : (int.Parse(InputDisplays[0].text) - 1)).ToString();
      }
      else {
         InputDisplays[1].text = ((int.Parse(InputDisplays[1].text) - 1) < 0 ? 9 : (int.Parse(InputDisplays[1].text) - 1)).ToString();
      }
   }

   #endregion

   #region Calculations

   void Start () {
      if (Rnd.Range(0, 2) == 0) {               //Randomizes which way primary is
         InitialLocation = Displays[0].transform.localPosition;
         Displays[0].transform.localPosition = Displays[1].transform.localPosition;
         Displays[1].transform.localPosition = InitialLocation;
      }
      GenerateSequences();
   }

   void Calculate () {
      for (int i = 0; i < 8; i++) {
         int[] SequenceShifter = new int[8];
         int TempSwap = 0;
         switch (ColorSequence[i]) {
            case 0:                             //R
               TempPrimary[i] += TempSecondary[i];
               break;
            case 1:                             //B
               TempPrimary[i] -= TempSecondary[i];
               break;
            case 2:                             //G
               TempPrimary[i] = TempPrimary[i] * TempSecondary[i] % 10;
               break;
            case 3:                             //Y
               if (TempSecondary[i] == TempPrimary[i] * 2 && TempSecondary[i] != 0) {  //Why the fuck does Math.Round not round .5 up
                  TempPrimary[i] = 1;
                  break;
               }
               if (TempSecondary[i] != 0) {
                  TempPrimary[i] = (int) Math.Round(((double) TempPrimary[i]) / TempSecondary[i]);
                  break;
               }
               break;
            case 4:                             //W
               for (int j = 1; j < 9; j++) {
                  SequenceShifter[j % 8] = TempSecondary[j - 1];
               }
               for (int j = 0; j < 8; j++) {
                  TempSecondary[j] = SequenceShifter[j];
               }
               break;
            case 5:                             //K
               for (int j = 1; j < 9; j++) {
                  SequenceShifter[j - 1] = TempSecondary[j % 8];
               }
               for (int j = 0; j < 8; j++) {
                  TempSecondary[j] = SequenceShifter[j];
               }
               break;
            case 6:                             //O
               for (int j = 1; j < 9; j++) {
                  SequenceShifter[j % 8] = TempPrimary[j - 1];
               }
               for (int j = 0; j < 8; j++) {
                  TempPrimary[j] = SequenceShifter[j];
               }
               break;
            case 7:                             //I
               for (int j = 1; j < 9; j++) {
                  SequenceShifter[j - 1] = TempPrimary[j % 8];
               }
               for (int j = 0; j < 8; j++) {
                  TempPrimary[j] = SequenceShifter[j];
               }
               break;
            case 8:                             //P
               TempPrimary[i] = 9 - TempPrimary[i];
               break;
            case 9:                             //C
               TempSwap = TempSecondary[i];
               TempSecondary[i] = TempPrimary[i];
               TempPrimary[i] = TempSwap;
               for (int j = 7; j > -1; j--) {
                  SequenceShifter[7 - j] = TempSecondary[j];
               }
               for (int j = 0; j < 8; j++) {
                  TempSecondary[j] = SequenceShifter[j];
               }
               break;
            case 10:                            //N
               TempSwap = TempSecondary[i];
               TempSecondary[i] = TempPrimary[i];
               TempPrimary[i] = TempSwap;
               for (int j = 7; j > -1; j--) {
                  SequenceShifter[7 - j] = TempPrimary[j];
               }
               for (int j = 0; j < 8; j++) {
                  TempPrimary[j] = SequenceShifter[j];
               }
               break;
         }
         Debug.LogFormat("[Quick Arithmetic #{0}] After the {1} modifier...", ModuleId, new string[] { "red", "blue", "green", "yellow", "white", "black", "orange", "pink", "purple", "cyan", "brown"}[ColorSequence[i]]);
         Debug.LogFormat("[Quick Arithmetic #{0}] The modified primary sequence is now {1} {2} {3} {4} {5} {6} {7} {8}.", ModuleId, TempPrimary[0], TempPrimary[1], TempPrimary[2], TempPrimary[3], TempPrimary[4], TempPrimary[5], TempPrimary[6], TempPrimary[7]);
         Debug.LogFormat("[Quick Arithmetic #{0}] The modified secondary sequence is now {1} {2} {3} {4} {5} {6} {7} {8}.", ModuleId, TempSecondary[0], TempSecondary[1], TempSecondary[2], TempSecondary[3], TempSecondary[4], TempSecondary[5], TempSecondary[6], TempSecondary[7]);
      }
      for (int i = 0; i < 8; i++) {
         TempPrimary[i] = Math.Abs(TempPrimary[i] + TempSecondary[i]);
         Answers[i] = Table[TempPrimary[i]];
      }
   }

   void GenerateSequences () {
      HOWRestart:
      for (int i = 0; i < 8; i++) {
         ColorSequence[i] = Rnd.Range(0, 11); //Generates colors, making sure not all are white
      }
      int ColorCheck = 0;
      for (int i = 0; i < 8; i++) {
         if (ColorSequence[i] == 4) {
            ColorCheck++;
         }
      }
      if (ColorCheck == 8) {
         goto HOWRestart;
      }
      GenerationHelper(LeftSequenceN);
      GenerationHelper(RightSequence);
      for (int i = 0; i < 8; i++) {
         TempPrimary[i] = LeftSequenceN[i];
         TempSecondary[i] = RightSequence[i];
      }
      if (Rnd.Range(0, 2) == 0) {//0 means left display is faster.
         LDisplaySequences = StartCoroutine(Flash(Displays[0], true, true));
         RDisplaySequences = StartCoroutine(Flash(Displays[1], false, false));
      }
      else {
         LDisplaySequences = StartCoroutine(Flash(Displays[0], false, true));
         RDisplaySequences = StartCoroutine(Flash(Displays[1], true, false));
      }
      Debug.LogFormat("[Quick Arithmetic #{0}] The generated primary sequence is {1}{2} {3}{4} {5}{6} {7}{8} {9}{10} {11}{12} {13}{14} {15}{16}.", ModuleId, "RBGYWKOIPCN"[ColorSequence[0]], LeftSequenceN[0], "RBGYWKOIPCN"[ColorSequence[1]], LeftSequenceN[1], "RBGYWKOIPCN"[ColorSequence[2]], LeftSequenceN[2], "RBGYWKOIPCN"[ColorSequence[3]], LeftSequenceN[3], "RBGYWKOIPCN"[ColorSequence[4]], LeftSequenceN[4], "RBGYWKOIPCN"[ColorSequence[5]], LeftSequenceN[5], "RBGYWKOIPCN"[ColorSequence[6]], LeftSequenceN[6], "RBGYWKOIPCN"[ColorSequence[7]], LeftSequenceN[7]);
      Debug.LogFormat("[Quick Arithmetic #{0}] The generated secondary sequence is {1}{2}{3}{4}{5}{6}{7}{8}.", ModuleId, RightSequence[0], RightSequence[1], RightSequence[2], RightSequence[3], RightSequence[4], RightSequence[5], RightSequence[6], RightSequence[7]);
      Calculate();
      Debug.LogFormat("[Quick Arithmetic #{0}] The answers are {1} {2} {3} {4} {5} {6} {7} {8}.", ModuleId, Answers[0].ToString("00"), Answers[1].ToString("00"), Answers[2].ToString("00"), Answers[3].ToString("00"), Answers[4].ToString("00"), Answers[5].ToString("00"), Answers[6].ToString("00"), Answers[7].ToString("00"));
   }
   
   void GenerationHelper (int[] Sequence) {
      Sequence[0] = Rnd.Range(0, 10);
      for (int i = 1; i < 8; i++) {
         do {
            Sequence[i] = Rnd.Range(0, 10);
         } while (Sequence[i] == Sequence[i - 1]); //Makes sure no digit is the same as the one before
      }
   }

   #endregion

   #region Animations

   IEnumerator Flash (TextMesh Text, bool IsFast, bool UseLeft) {
      int Prog = Rnd.Range(0, 8);
      while (true) {
         if (Prog == 8) {
            Text.text = "";
         }
         else {
            if (UseLeft) {
               Text.text = LeftSequenceN[Prog].ToString();
               Text.color = new Color32(Colors[ColorSequence[Prog]][0], Colors[ColorSequence[Prog]][1], Colors[ColorSequence[Prog]][2], 255);
            }
            else {
               Text.text = RightSequence[Prog].ToString();
            }
         }
         Prog = (Prog + 1) % 9;
         FinalColor = Prog;
         if (IsFast) {
            yield return new WaitForSeconds(Rnd.Range(.5f, .7f));
         }
         else {
            yield return new WaitForSeconds(Rnd.Range(.7f, .9f));
         }
      }
   }

   IEnumerator Fade () {
      while (Opacity != 0) {
         Displays[0].color = new Color32(Colors[ColorSequence[FinalColor]][0], Colors[ColorSequence[FinalColor]][1], Colors[ColorSequence[FinalColor]][2], Opacity);
         Displays[1].color = new Color32(255, 255, 255, Opacity);
         for (int i = 0; i < 2; i++) {
            InputDisplays[i].color = new Color32(255, 255, 255, Opacity);
         }
         Opacity--;
         yield return new WaitForSeconds(0.01f);
      }
   }

   #endregion

   #region TP

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} ## to submit a number.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
      yield return null;
      if (Command.RegexMatch("[0-9][0-9]")) {
         if (int.Parse(InputDisplays[0].text) - int.Parse(Command[0].ToString()) != 0) {
            if ((int.Parse(InputDisplays[0].text) - int.Parse(Command[0].ToString()) <= 5 && int.Parse(InputDisplays[0].text) - int.Parse(Command[0].ToString()) > 0) || (int.Parse(InputDisplays[0].text) - int.Parse(Command[0].ToString()) <= -5 && int.Parse(InputDisplays[0].text) - int.Parse(Command[0].ToString()) < 0)) {
               while (Command[0].ToString() != InputDisplays[0].text) {
                  Arrows[2].OnInteract();
                  yield return new WaitForSeconds(.1f);
               }
            }
            else {
               while (Command[0].ToString() != InputDisplays[0].text) {
                  Arrows[0].OnInteract();
                  yield return new WaitForSeconds(.1f);
               }
            }
         }
         if (int.Parse(InputDisplays[1].text) - int.Parse(Command[1].ToString()) != 0) {
            if (int.Parse(InputDisplays[1].text) - int.Parse(Command[1].ToString()) >= 5 || int.Parse(InputDisplays[1].text) - int.Parse(Command[1].ToString()) <= -5) {
               while (Command[1].ToString() != InputDisplays[1].text) {
                  Arrows[3].OnInteract();
                  yield return new WaitForSeconds(.1f);
               }
            }
            else {
               while (Command[1].ToString() != InputDisplays[1].text) {
                  Arrows[1].OnInteract();
                  yield return new WaitForSeconds(.1f);
               }
            }
         }
         Submit.OnInteract();
         yield return new WaitForSeconds(.1f);
      }
      else {
         yield return "sendtochaterror I don't understand!";
      }
   }

   IEnumerator TwitchHandleForcedSolve () {
      for (int i = 0; i < 8; i++) {
         yield return ProcessTwitchCommand(Answers[i].ToString("00"));
      }
   }
   #endregion
}
