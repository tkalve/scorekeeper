using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Topshelf;

namespace ScoreHub
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<ScoreHub>(s =>
                {
                    s.ConstructUsing(name => new ScoreHub());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("ScoreKeeper ScoreHub");
                x.SetDisplayName("ScoreHub");
                x.SetServiceName("ScoreHub");
            });
        }
    }
}
