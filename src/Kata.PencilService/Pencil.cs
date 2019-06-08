using System;
using System.Text;

namespace Kata.PencilService
{
    public class Pencil
    {
        public int InitialDurability { get; private set; }
        public int Durability {get; private set;}
        public int Length { get; private set; }

        private const string NonBreakSpace = "&nbsp;";

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

        public void Sharpen()
        {
            if (Length > 0)
            {
                Length -= 1;
                Durability = InitialDurability;
            }
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
