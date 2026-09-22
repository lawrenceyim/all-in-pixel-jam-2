using System;

namespace AddOns.Repository;

public interface IRepository<TId> where TId : struct, Enum {
    static abstract string ValidateUids();
    static abstract string GetUid(TId id);
}