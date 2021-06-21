using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Services
{
    public interface IGenericCRUD<T> where T : class
    {
        public Task<IEnumerable<T>> GetAll();

        public Task<T> GetDetails(int id);

        public Task<T> Create(T obj);

        public Task<T> Update(int id,T obj);

        public Task<T> Delete(int id);
    }

    public class GenericCRUD<T> : IGenericCRUD<T> where T : BaseModel
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public GenericCRUD(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<T> Create(T obj)
        {
            GetDB.Add(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> Delete(int id)
        {
            var obj = await GetDB.FirstOrDefaultAsync(x => x.Id == id);
            if (obj != null)
            {
                GetDB.Remove(obj);
            }
            return obj;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAll()
        {
            return await GetDB.ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetDetails(int id)
        {
            return await GetDB.FirstOrDefaultAsync(x=>x.Id==id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<T> Update(int id, T obj)
        {
            obj.Id = id;
            _context.Entry(obj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObjectExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return obj;
        }

        #region helper
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool ObjectExists(int id)
        {
            //.Set<Object>() setea la clase genérica
            //return _context.Set<T>().Any(e => e.Id == id);
            return GetDB.Any(e => e.Id == id);
        }

        private DbSet<T> GetDB => _context.Set<T>();

        #endregion

        
    }
}
