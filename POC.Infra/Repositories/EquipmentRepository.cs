using MongoDB.Driver;
using POC.Domain.Entities;
using POC.Domain.Repositories;
using POC.Infra.Persistence;

namespace POC.Infra.Repositories
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly IMongoCollection<Equipment> _equipments;

        public EquipmentRepository(MongoDbContext context)
        {
            _equipments = context.Equipments;
        }

        public async Task AddAsync(Equipment equipment)
        {
            await _equipments.InsertOneAsync(equipment);
        }

        public async Task<Equipment> GetByIdAsync(string id)
        {
            return await _equipments.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Equipment>> GetAllAsync()
        {
            return await _equipments.Find(_ => true).ToListAsync();
        }
    }
}
