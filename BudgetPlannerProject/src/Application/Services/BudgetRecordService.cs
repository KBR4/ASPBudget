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
        private IMapper mapper;
        public BudgetRecordService(IBudgetRecordRepository budgetRecordRepository, IMapper mapper)
        {
            this._budgetRecordRepository = budgetRecordRepository;
            this.mapper = mapper;
        }
        public async Task<int> Add(BudgetRecordDto budgetRecord)
        {
            var mappedbudgetRecord = mapper.Map<BudgetRecord>(budgetRecord);
            if (mappedbudgetRecord != null)
            {
                await _budgetRecordRepository.Create(mappedbudgetRecord);
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
            var mappedbudgetRecords = budgetRecords.Select(q => mapper.Map<BudgetRecordDto>(q)).ToList();
            return mappedbudgetRecords;
        }

        public async Task<BudgetRecordDto?> GetById(int id)
        {
            var budgetRecord = await _budgetRecordRepository.ReadById(id);
            var mappedbudgetRecord = mapper.Map<BudgetRecordDto>(budgetRecord);
            return mappedbudgetRecord;
        }

        public async Task<bool> Update(BudgetRecordDto budgetRecord)
        {
            var mappedbudgetRecord = mapper.Map<BudgetRecord>(budgetRecord);
            return await _budgetRecordRepository.Update(mappedbudgetRecord);
        }
    }
}
