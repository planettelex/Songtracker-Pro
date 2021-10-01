namespace SongtrackerPro.Data.Enums
{
    public enum ContractStatus
    {
        Unspecified,
        Drafted,
        Provided,
        Proposed,
        Executed,
        Rejected,
        Expired
    }

    public static class ContractStatusesFor
    {
        public static ContractStatus[] Phase(ContractPhase contractPhase)
        {
            return contractPhase switch
            {
                ContractPhase.Create => new[] { ContractStatus.Drafted, ContractStatus.Provided },
                ContractPhase.Negotiate => new[] { ContractStatus.Proposed },
                ContractPhase.Active => new[] { ContractStatus.Executed },
                ContractPhase.Obsolete => new[] { ContractStatus.Rejected, ContractStatus.Expired },
                _ => null
            };
        }
    }
}
