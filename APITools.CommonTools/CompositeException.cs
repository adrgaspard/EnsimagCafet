using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITools.CommonTools
{
    public sealed class CompositeException : Exception
    {
        public CompositeException(IEnumerable<Exception> innerExceptions)
        {
            InnerExceptions = new ReadOnlyCollection<Exception>((innerExceptions ?? Array.Empty<Exception>()).ToList());
        }

        public IReadOnlyList<Exception> InnerExceptions { get; private init; }
    }
}
