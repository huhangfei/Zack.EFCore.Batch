﻿using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo
{
    class Program
    {

        static async Task Main(string[] args)
        {
            using (TestDbContext ctx = new TestDbContext())
            {
                
                int n = Convert.ToInt32("3");
                string s = "hello";
                //ctx.Database.BeginTransaction();
                
                await ctx.DeleteRangeAsync<Book>(b => b.Price > n || b.AuthorName == s,true);
                await ctx.BatchUpdate<Book>()
                    .Set(b => b.Price, b => b.Price + 3)
                    .Set(b => b.Title, b => s)
                    .Set(b=>b.AuthorName,b=>b.Title.Substring(3,2))
                    //b.Title.Substring(3,2)+b.AuthorName.ToUpper() is not supported on Npgsql.EntityFrameworkCore.PostgreSQL 5.0.0, so I cannot either.
                    .Set(b => b.PubTime, b => DateTime.Now)
                    .Where(b => b.Id > n || b.AuthorName.StartsWith("Zack"))
                    .ExecuteAsync();
            }
        }
    }
}
