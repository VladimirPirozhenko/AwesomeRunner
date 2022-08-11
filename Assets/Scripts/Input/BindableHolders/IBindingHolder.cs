using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IBindingHolder<T> where T : IBinding
{
    public Dictionary<ECommand, T> InputBindings { get; }
    public void Init();
}

