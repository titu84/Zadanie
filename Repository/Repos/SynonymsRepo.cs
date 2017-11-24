using DbDomain.Models;
using DbDomain.ViewModels;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace Repository.Repos
{
    public class SynonymsRepo : IDisposable
    {
        SynonymDb db = new SynonymDb();
        public async Task<int?> AddSynonym(Synonym synonym)
        {
            synonym.Synonyms = trimSynonyms(synonym.Synonyms); // Usuwanie spacji jeśli wprowadzone.
            try
            {
                await Task.Run(()=>db.Synonyms.Add(synonym));
                await db.SaveChangesAsync();               
                return synonym.Id;
            }
            catch
            {
                return -1;
            }
        }
        public async Task<SynonymViewModel> GetSynonymVM(int? id)
        {
            var tempSyn = await Task.Run(() => db.Synonyms.Where(a => a.Id == id).FirstOrDefault());
            if (tempSyn != null && tempSyn.Synonyms != null)
            {
                var result = new SynonymViewModel();
                try
                {
                    result.Synonym = tempSyn;
                    result.SynonymsList = splitHelper(tempSyn.Synonyms);
                    result.Message = "OK";
                    return result;
                }
                catch (Exception ex)
                {
                    return new SynonymViewModel() { Message = ex.Message };
                }
            }
            else
                return new SynonymViewModel() { Message = "Nie znaleziono" };

        }
        List<string> splitHelper(string input)
        {
            var result = input.Split(',').ToList();
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].Length == 0 || String.IsNullOrWhiteSpace(result[i]))
                {
                    result.Remove(result[i]);
                }
            }          
            return result;
        }
        string trimSynonyms(string input)
        {
            StringBuilder s = new StringBuilder();
            foreach (var item in splitHelper(input))
            {
                s.Append(item.Trim()).Append(",");
            }
            return s.ToString();
        }
        #region IDisposable
        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                handle.Dispose();
            }
            disposed = true;
        }
        #endregion
    }
}
