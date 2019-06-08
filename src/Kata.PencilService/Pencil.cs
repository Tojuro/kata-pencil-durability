using System;
using System.Text;

namespace Kata.PencilService
{
    public class Pencil
    {
        public int InitialDurability { get; private set; }
        public int Durability {get; private set;}
        public int Length { get; private set; }

        private const string NonBreakSpace = " ";

        public Pencil (int durability = 1000, int length = 5)
        {
            InitialDurability = durability > 0 ? durability : 1000; 
            Durability = durability;
            Length = length;
        }

        public void Write (string text, ref Paper paper)
        {
            var displayText = ApplyTextDegradation(text);
            paper.Content = string.Concat(paper.Content, displayText);
        }

        public void Erase(string text, ref Paper paper)
        {
            var idx = paper.Content.LastIndexOf(text);

            if (idx >= 0)
            {
                var newText = Spacify(text);

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

        private string Spacify(string text)
        {
            var len = text.Length;
            var ret = new StringBuilder();

            //TODO: I know there is a better way to do this, but I have no internets right now
            for (int i = 0; i < len; i++)
            {
                ret.Append(" ");
            }
            return ret.ToString();
        }

        private string ApplyTextDegradation(string text)
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

        private string PointDegradation(char chr, int ptVal)
        {
            var canWrite = Durability >= ptVal && ptVal > 0;
            var isSpace = char.IsWhiteSpace(chr);
            Durability = Math.Max(0, Durability - ptVal);

            return canWrite && !isSpace ? chr.ToString() : NonBreakSpace;
        }
    }
}
