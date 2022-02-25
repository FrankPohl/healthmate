using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace HealthMate.Services
{
        public interface IMicService

        {
            Task<bool> GetPermissionsAsync();
            void OnRequestPermissionsResult(bool isGranted);
        }
}
