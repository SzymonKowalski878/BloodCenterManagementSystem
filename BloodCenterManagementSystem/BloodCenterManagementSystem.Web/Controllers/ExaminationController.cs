﻿using AutoMapper;
using BloodCenterManagementSystem.Logics;
using BloodCenterManagementSystem.Logics.Interfaces;
using BloodCenterManagementSystem.Models;
using BloodCenterManagementSystem.Web.DTO.ResultOfExamination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodCenterManagementSystem.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExaminationController:Controller
    {
        private readonly Lazy<IResultOfExaminationLogic> _resultOfExamintaionLogic;
        protected IResultOfExaminationLogic ResultOfExaminationLogic => _resultOfExamintaionLogic.Value;

        private readonly Lazy<IMapper> _mapper;
        protected IMapper Mapper => _mapper.Value;

        public ExaminationController(Lazy<IResultOfExaminationLogic> resultOfExaminationLogic,
            Lazy<IMapper> mapper)
        {
            _resultOfExamintaionLogic = resultOfExaminationLogic;
            _mapper = mapper;
        }

        [Authorize(Policy = "Worker")]
        [HttpPost, Route("AddExamination")]
        [ProducesResponseType(typeof(AddResultOfExaminationDTO), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult AddExamination(AddResultOfExaminationDTO data)
        {
            var dataToAdd = Mapper.Map<AddResultOfExaminationDTO, ResultOfExaminationModel>(data);

            var result = ResultOfExaminationLogic.AddResults(dataToAdd);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(data);
        }

        [Authorize(Policy = "Worker")]
        [HttpPost,Route("UpdateExamination")]
        [ProducesResponseType(typeof(UpdateResultOfExaminationDTO), 200)]
        [ProducesResponseType(typeof(IEnumerable<ErrorMessage>), 400)]
        public IActionResult UpdateExamination(UpdateResultOfExaminationDTO data)
        {
            var dataToAdd = Mapper.Map<UpdateResultOfExaminationDTO, ResultOfExaminationModel>(data);

            var result = ResultOfExaminationLogic.UpdateResults(dataToAdd);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(data);
        }
    }
}
