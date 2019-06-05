using System;

namespace Kata.PencilService
{
    public class Pencil
    {
        public void Write (string text, ref Paper paper)
        {
            paper.Content = string.Concat(paper.Content, text);
        }
    }
}
