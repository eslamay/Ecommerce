﻿using AutoMapper;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BaseController : ControllerBase
	{
		protected readonly IUnitOfWork work;
		protected readonly IMapper mapper;

		public BaseController(IUnitOfWork work,
			IMapper mapper)
		{
			this.work = work;
			this.mapper = mapper;
		}
	}
}
