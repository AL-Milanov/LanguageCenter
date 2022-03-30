﻿namespace LanguageCenter.Core.Data.Repository
{
    public interface IRepository
    {
        IQueryable<T> GetAll<T>() where T : class;

        Task<T> GetByIdAsync<T>(string id) where T :  class;

        Task<bool> Delete<T>(string id) where T: class;

        Task SaveChangesAsync();
    }
}