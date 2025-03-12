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
        private IBudgetRecordRepository budgetRecordRepository;
        private IMapper mapper;
        public BudgetRecordService(IBudgetRecordRepository budgetRecordRepository, IMapper mapper)
        {
            this.budgetRecordRepository = budgetRecordRepository;
            this.mapper = mapper;
        }
        public async Task Add(BudgetRecordDto budgetRecord)
        {
            var mappedbudgetRecord = mapper.Map<BudgetRecord>(budgetRecord);
            if (mappedbudgetRecord != null)
            {
                await budgetRecordRepository.Create(mappedbudgetRecord);
            }
        }

        public async Task<bool> Delete(int id)
        {
            return await budgetRecordRepository.Delete(id);
        }

        public async Task<List<BudgetRecordDto>> GetAll()
        {
            var budgetRecords = await budgetRecordRepository.ReadAll();
            var mappedbudgetRecords = budgetRecords.Select(q => mapper.Map<BudgetRecordDto>(q)).ToList();
            return mappedbudgetRecords;
        }

        public async Task<BudgetRecordDto?> GetById(int id)
        {
            var budgetRecord = await budgetRecordRepository.ReadById(id);
            var mappedbudgetRecord = mapper.Map<BudgetRecordDto>(budgetRecord);
            return mappedbudgetRecord;
        }

        public async Task<bool> Update(BudgetRecordDto budgetRecord)
        {
            var mappedbudgetRecord = mapper.Map<BudgetRecord>(budgetRecord);
            return await budgetRecordRepository.Update(mappedbudgetRecord);
        }
    }
}
