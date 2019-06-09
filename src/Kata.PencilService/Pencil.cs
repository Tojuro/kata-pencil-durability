using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kata.PencilService
{
    public class Pencil
    {
        public int InitialDurability { get; private set; }
        public int Durability {get; private set;}
        public int Length { get; private set; }
        public int EraserDurability { get; private set; }

        private int _lastErasedIndex { get; set; }

        private const string NonBreakSpace = " ";

        public Pencil (int durability = 1000, int length = 5, int eraserDurability = 1000)
        {
            InitialDurability = durability > 0 ? durability : 1000; 
            Durability = durability;
            Length = length;
            EraserDurability = eraserDurability;
        }

        public void Write (string text, ref Paper paper)
        {
            var displayText = ApplyWriteDegradation(text);
            paper.Content = string.Concat(paper.Content, displayText);
        }

        public void Rewrite(string text, ref Paper paper)
        {
            var displayText = ApplyWriteDegradation(text);

            var newText = OverwriteText(paper.Content.Substring(_lastErasedIndex, text.Length), text);
            var preString = paper.Content.Substring(0, _lastErasedIndex);
            var postString = paper.Content.Substring(_lastErasedIndex + newText.Length);

            paper.Content = string.Concat(preString, newText, postString);
        }

        public void Erase(string text, ref Paper paper)
        {
            var idx = paper.Content.LastIndexOf(text);

            if (idx >= 0)
            {
                _lastErasedIndex = idx;

                var newText = ApplyEraseDegradation(idx, text);

                var preString = paper.Content.Substring(0, idx);
                var postString = paper.Content.Substring(idx + newText.Length);

                paper.Content = string.Concat(preString, newText, postString);
            }
        }

        public void Sharpen()
        {
            if (Length > 0)
            {
                Length -= 1;
                Durability = InitialDurability;
            }
        }

        private string OverwriteText(string currentText, string replaceText)
        {
            var output = new StringBuilder();
            var currentChars = currentText.ToCharArray();
            var replaceChars = replaceText.ToCharArray();

            for(int i = 0; i < currentText.Length; i++)
            {
                if (currentChars[i].Equals(' '))
                {
                    output.Append(replaceChars[i]);
                }
                else if (!char.IsWhiteSpace(currentChars[i]) && !char.IsWhiteSpace(replaceChars[i]))
                {
                    output.Append('@');
                }
                else
                {
                    output.Append(currentChars[i]);
                }
            }

            return output.ToString();
        }

        private string ApplyEraseDegradation(int index, string erasedText)
        {
            var outputArray = new List<char>();
            var chrs = erasedText.ToCharArray();

            for (var i = erasedText.Length-1; i >= 0; i--)
            {
                var ptValue = (char.IsWhiteSpace(chrs[i]) ? 0 : 1);
                outputArray.Add(EraserDegradation(chrs[i], ptValue));
            }

            outputArray.Reverse();

            return string.Join(string.Empty, outputArray);
        }

        private char EraserDegradation(char chr, int ptValue)
        {
            var canWrite = EraserDurability >= ptValue && ptValue > 0;
            var isSpace = char.IsWhiteSpace(chr);
            EraserDurability = Math.Max(0, EraserDurability - ptValue);

            return canWrite && !isSpace ? ' ' : chr;
        }

        private string ApplyWriteDegradation(string text)
        {
            var displayText = new StringBuilder();
            var chrs = text.ToCharArray();

            foreach(var chr in chrs)
            {
                // NOTE: treating lowercase (per spec), numbers and special characters == 1
                var ptValue = (!char.IsWhiteSpace(chr) ? 1 : 0) + (char.IsUpper(chr) ? 1 : 0);
                displayText.Append(PointDegradation(chr, ptValue));
            }

            return displayText.ToString();
        }

        private string PointDegradation(char chr, int ptValue)
        {
            var canWrite = Durability >= ptValue && ptValue > 0;
            var isSpace = char.IsWhiteSpace(chr);
            Durability = Math.Max(0, Durability - ptValue);

            return canWrite && !isSpace ? chr.ToString() : NonBreakSpace;
        }
    }
}
