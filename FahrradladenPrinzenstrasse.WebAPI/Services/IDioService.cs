using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public interface IDioService
    {
        List<Model.Dio> Get(Model.Requests.DioSearchRequest request);
        Model.Dio GetById(int id);
    }
}
