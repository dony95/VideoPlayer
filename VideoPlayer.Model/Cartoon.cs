using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoPlayer.Model
{
    public class Cartoon : Video
    {
        public Language Language { get; set; }
    }

    public enum Language
    {
        Hrvatski = 1, Engleski, Njemački
    }
}
