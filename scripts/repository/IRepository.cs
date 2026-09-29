using System;

namespace AddOns.Repository;

public interface IRepository<TId> where TId : struct, Enum {
    static abstract string ValidateUids();
    // Add a default scene that's an ? or something
    static abstract string GetUid(TId id);
}