﻿using EMS.Core.Data;
using EMS.Users.API.Business.Interfaces.Repository;
using EMS.Users.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Users.API.Data.Repository;

public class SubscriberRepository : ISubscriberRepository
{
    private readonly UsersContext _context;

    public SubscriberRepository(UsersContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task<IEnumerable<Subscriber>> GetAllSubscribers()
    {
        return await _context.Subscribers.AsNoTracking().ToListAsync();
    }

    public async Task<Subscriber> GetById(Guid id)
    {
        return await _context.Subscribers.FirstOrDefaultAsync(c => c.Id == id) ?? null!;
    }

    public async Task<Subscriber> GetByCpf(string cpf)
    {
        return await _context.Subscribers.FirstOrDefaultAsync(c => c.Cpf.Number == cpf) ?? null!;
    }

    public void AddSubscriber(Subscriber subscriber)
    {
        _context.Subscribers.Add(subscriber);
    }

    public void UpdateSubscriber(Subscriber subscriber)
    {
        _context.Subscribers.Update(subscriber);
    }

    public async Task<bool> DeleteSubscriber(Subscriber subscriber)
    {
        var subscriberDb = await _context.Subscribers
            .Include(s => s.Workers)
            .Include(s => s.Clients)
            .FirstOrDefaultAsync(s => s.Id == subscriber.Id);

        if (subscriber == null!)
        {
            return false;
        }
        _context.Subscribers.Remove(subscriber);
        return true;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}