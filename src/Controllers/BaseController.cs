using AutoMapper;
using ICICI.AppCode.DAL;
using ICICI.AppCode.Interfaces;
using ICICI.AppCode.Reops.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ICICI.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected IDapperRepository _dapper;
        protected IMapper _mapper;
        public BaseController(IDapperRepository dapper,IMapper mapper)
        {
            _dapper = dapper;
            _mapper = mapper;
        }
    }
}
