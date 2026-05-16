using ChurchSermonsMetaData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchSermonsMetaData.Data
{
    public class SermonStore
    {
        private SermonInfo _state = new();

        public SermonInfo State => _state;

        public event EventHandler<SermonInfo>? StateChanged;

        public void Update(Action<SermonInfo> updateAction)
        {
            updateAction(_state);
            StateChanged?.Invoke(this, _state);
        }
    }
}
