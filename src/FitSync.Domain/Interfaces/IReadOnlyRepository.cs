using FitSync.Domain.Entities;

namespace FitSync.Domain.Interfaces;

public interface IReadOnlyRepository<TEntity> where TEntity : Entity
{
}