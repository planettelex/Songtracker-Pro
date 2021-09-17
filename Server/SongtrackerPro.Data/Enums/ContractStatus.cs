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
            switch (contractPhase)
            {
                case ContractPhase.Create:
                    return new[] { ContractStatus.Drafted, ContractStatus.Provided };
                case ContractPhase.Negotiate:
                    return new[] { ContractStatus.Proposed };
                case ContractPhase.Active:
                    return new[] { ContractStatus.Executed };
                case ContractPhase.Obsolete:
                    return new[] { ContractStatus.Rejected, ContractStatus.Expired };
            }

            return null;
        }
    }
}
