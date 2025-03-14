using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BudgetRecordService : IBudgetRecordService
    {
        private IBudgetRecordRepository _budgetRecordRepository;
        private IMapper _mapper;

        public BudgetRecordService(IBudgetRecordRepository budgetRecordRepository, IMapper mapper)
        {
            _budgetRecordRepository = budgetRecordRepository;
            _mapper = mapper;
        }

        public async Task<int> Add(BudgetRecordDto budgetRecord)
        {
            var mappedBudgetRecord = _mapper.Map<BudgetRecord>(budgetRecord);
            if (mappedBudgetRecord != null)
            {
                await _budgetRecordRepository.Create(mappedBudgetRecord);
            }
            return -1;
        }

        public async Task<bool> Delete(int id)
        {
            return await _budgetRecordRepository.Delete(id);
        }

        public async Task<List<BudgetRecordDto>> GetAll()
        {
            var budgetRecords = await _budgetRecordRepository.ReadAll();
            var mappedBudgetRecords = budgetRecords.Select(q => _mapper.Map<BudgetRecordDto>(q)).ToList();
            return mappedBudgetRecords;
        }

        public async Task<BudgetRecordDto?> GetById(int id)
        {
            var budgetRecord = await _budgetRecordRepository.ReadById(id);
            var mappedBudgetRecord = _mapper.Map<BudgetRecordDto>(budgetRecord);
            return mappedBudgetRecord;
        }

        public async Task<bool> Update(BudgetRecordDto budgetRecord)
        {
            if (budgetRecord == null)
            {
                return false;
            }
            var mappedBudgetRecord = _mapper.Map<BudgetRecord>(budgetRecord);
            return await _budgetRecordRepository.Update(mappedBudgetRecord);
        }
    }
}
