using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPlayer.Model;

namespace VideoPlayer.DAL.Repository
{
    public interface IFilmFilter
    {
		string Name { get; }
		int Year { get; }
		Category Category { get; }
        Language Language { get; }
    }
}
