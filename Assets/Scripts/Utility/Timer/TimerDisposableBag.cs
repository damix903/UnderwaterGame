using System;
using System.Collections.Generic;

namespace Underwater.Utility.Timer
{
    public class TimerDisposableBag : IDisposable
    {
        private readonly List<IDisposable> _disposables = new();
        
        public void Add(IDisposable disposable) => _disposables.Add(disposable);
        
        public void Dispose()
        {
            foreach (var disposable in _disposables) disposable.Dispose();
        }
    }
}