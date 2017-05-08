// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus
{
    /// <summary>
    /// Writes text in a dialog box.
    /// </summary>
    [CommandInfo("Narrative",
                 "Say",
                 "Writes text in a dialog box and says it outloud. Test")]
    [AddComponentMenu("")]
    public class Say : Command, ILocalizable
    {
        //Set your required pitch on every say dialog block or change default from here
        [SerializeField]
        protected float pitch = 0f;
        //Set your required speech Speed on every say dialog block or change default from here
        [SerializeField]
        protected float speechSpeed = 1.3f;
        [Tooltip("Select 1 for English | 2 for German")]
        public int language = 2;


        // Removed this tooltip as users's reported it obscures the text box
        [TextArea(5, 10)]
        [SerializeField]
        protected string storyText = "";

        [Tooltip("Notes about this story text for other authors, localization, etc.")]
        [SerializeField]
        protected String description = "";

        [Tooltip("Character that is speaking")]
        [SerializeField]
        protected Character character;

        [Tooltip("Portrait that represents speaking character")]
        [SerializeField]
        protected Sprite portrait;

        [Tooltip("Voiceover audio to play when writing the text")]
        [SerializeField]
        protected AudioClip voiceOverClip;

        [Tooltip("Always show this Say text when the command is executed multiple times")]
        [SerializeField]
        protected bool showAlways = true;

        [Tooltip("Number of times to show this Say text when the command is executed multiple times")]
        [SerializeField]
        protected int showCount = 1;

        [Tooltip("Type this text in the previous dialog box.")]
        [SerializeField]
        protected bool extendPrevious = false;

        [Tooltip("Fade out the dialog box when writing has finished and not waiting for input.")]
        [SerializeField]
        protected bool fadeWhenDone = true;

        [Tooltip("Wait for player to click before continuing.")]
        [SerializeField]
        protected bool waitForClick = true;

        [Tooltip("Stop playing voiceover when text finishes writing.")]
        [SerializeField]
        protected bool stopVoiceover = true;

        [Tooltip("Sets the active Say dialog with a reference to a Say Dialog object in the scene. All story text will now display using this Say Dialog.")]
        [SerializeField]
        protected SayDialog setSayDialog;

        

        protected int executionCount;

        #region Public members
        
        private Locale[] languages = { Locale.FRENCH, Locale.ENGLISH, Locale.GERMAN };
        private string output;





        /// <summary>
        /// Character that is speaking.
        /// </summary>
        public virtual Character _Character { get { return character; } }

        /// <summary>
        /// Portrait that represents speaking character.
        /// </summary>
        public virtual Sprite Portrait { get { return portrait; } set { portrait = value; } }

        /// <summary>
        /// Type this text in the previous dialog box.
        /// </summary>
        public virtual bool ExtendPrevious { get { return extendPrevious; } }

        public override void OnEnter()
        {
            output = null;

            if (!showAlways && executionCount >= showCount)
            {
                Continue();
                return;
            }

            executionCount++;

            // Override the active say dialog if needed
            if (character != null && character.SetSayDialog != null)
            {
                SayDialog.ActiveSayDialog = character.SetSayDialog;
            }

            if (setSayDialog != null)
            {
                SayDialog.ActiveSayDialog = setSayDialog;
            }

            var sayDialog = SayDialog.GetSayDialog();
            if (sayDialog == null)
            {
                Continue();
                return;
            }

            var flowchart = GetFlowchart();

            sayDialog.SetActive(true);

            sayDialog.SetCharacter(character);
            sayDialog.SetCharacterImage(portrait);

            string displayText = storyText;

            var activeCustomTags = CustomTag.activeCustomTags;
            for (int i = 0; i < activeCustomTags.Count; i++)
            {
                var ct = activeCustomTags[i];
                displayText = displayText.Replace(ct.TagStartSymbol, ct.ReplaceTagStartWith);
                if (ct.TagEndSymbol != "" && ct.ReplaceTagEndWith != "")
                {
                    displayText = displayText.Replace(ct.TagEndSymbol, ct.ReplaceTagEndWith);
                }
            }

            string subbedText = flowchart.SubstituteVariables(displayText);

            sayDialog.Say(subbedText, !extendPrevious, waitForClick, fadeWhenDone, stopVoiceover, voiceOverClip, delegate {
                Continue();
            });


            //My Stuff
            try
            {
                String input = subbedText;
                StartCoroutine(waitFunction(input));

                SetLanguage(language);

                Debug.Log(output);

                SpeechEngine.AddProperties(pitch, speechSpeed);
                SpeechEngine.Speak(output);
            }
            catch
            {
                Debug.Log("This part isnt working. Probably because speech stuff doesnt work in editor. ");
            }

        }

        public override string GetSummary()
        {
            string namePrefix = "";
            if (character != null)
            {
                namePrefix = character.NameText + ": ";
            }
            if (extendPrevious)
            {
                namePrefix = "EXTEND" + ": ";
            }
            return namePrefix + "\"" + storyText + "\"";
        }

        public override Color GetButtonColor()
        {
            return new Color32(184, 210, 235, 255);
        }

        public override void OnReset()
        {
            executionCount = 0;
        }

        public override void OnStopExecuting()
        {
            var sayDialog = SayDialog.GetSayDialog();
            if (sayDialog == null)
            {
                return;
            }

            sayDialog.Stop();
        }

        #endregion

        #region ILocalizable implementation

        public virtual string GetStandardText()
        {
            return storyText;
        }

        public virtual void SetStandardText(string standardText)
        {
            storyText = standardText;
        }

        public virtual float GetPitch()
        {
            return pitch;
        }

        public virtual string GetStringId()
        {
            // String id for Say commands is SAY.<Localization Id>.<Command id>.[Character Name]
            string stringId = "SAY." + GetFlowchartLocalizationId() + "." + itemId + ".";
            if (character != null)
            {
                stringId += character.NameText;
            }

            return stringId;
        }

        #endregion


        //Removes "< >" and anything in between as Fungus creates these and these mess with the speech Engine
        //Removes "( )" and anything in between to display text on screen and not have speech engine say it outloud
        private IEnumerator waitFunction(String input)
        {
            string regex = "(\\<.*\\>)";
            output = System.Text.RegularExpressions.Regex.Replace(input, regex, string.Empty);
            output = System.Text.RegularExpressions.Regex.Replace(input, @" ?\(.*?\)", string.Empty);
            yield return new WaitForSeconds(10f);
        }


        //Sets language of the speech engine
        //0 is french, 1 is English, 2 is German
        public void SetLanguage(int index)
        {
            SpeechEngine.SetLanguage(languages[index]);
        }

        string ILocalizable.GetDescription()
        {
            throw new NotImplementedException();
        }
    }
}