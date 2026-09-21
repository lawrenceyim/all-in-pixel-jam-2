using System;

namespace Repository;

public interface IRepository<TId> where TId : struct, Enum {
    static abstract string ValidateUids();
    static abstract string GetUid(TId id);
}