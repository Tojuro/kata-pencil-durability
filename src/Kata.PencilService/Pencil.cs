using System;
using System.Text;

namespace Kata.PencilService
{
    public class Pencil
    {
        public int Durability {get; private set;}

        private const string NonBreakSpace = "&nbsp;";

        public Pencil (int durability = 1000)
        {
            Durability = durability;
        }

        public void Write (string text, ref Paper paper)
        {
            var displayText = ApplyTextDegradation(text);
            paper.Content = string.Concat(paper.Content, displayText);
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
