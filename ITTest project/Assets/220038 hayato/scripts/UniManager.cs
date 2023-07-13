using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace UniLang
{
    public class UniManager : MonoBehaviour
    {
        private Text thistext;
        private string oldtext;
        private bool converttrg = false;
        // Start is called before the first frame update
        void Start()
        {
            thistext = this.GetComponent<Text>();
            oldtext = thistext.text;
            converttrg = true;
            if(GManager.instance.isEnglish != 0) Invoke(nameof(LangConvert), 0.08f);
        }

        // Update is called once per frame
        void Update()
        {
            if (thistext.text != oldtext && !converttrg && GManager.instance.isEnglish!=0)
            {
                converttrg = true;
                oldtext = thistext.text;
                LangConvert();
            }
        }

        void LangConvert()
        {
            string[] tmparrey = oldtext.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
            thistext.text = "";
            oldtext = "";
            for (int i = 0; i < tmparrey.Length;)
            {
                var translator = Translator.Create(Language.Auto, Language.English);
                translator.Run(tmparrey[i], results =>
                {
                    foreach (var n in results)
                    {
                        thistext.text += n.translated +"\n";
                        oldtext += n.translated+ "\n";
                    }
                    thistext.text.TrimEnd('\r', '\n');
                    oldtext.TrimEnd('\r', '\n');
                });
                i++;
            }
            converttrg = false;
        }
    }
}
