namespace SongtrackerPro.Data.Enums
{
    public enum ContractPhase
    {
        Unspecified = 0,
        Create = 1,
        Negotiate = 2,
        Active = 3,
        Obsolete = 4
    }

    public static class ContractPhaseOf
    {
        public static ContractPhase Status(ContractStatus contractStatus)
        {
            switch (contractStatus)
            {
                case ContractStatus.Drafted:
                case ContractStatus.Provided:
                    return ContractPhase.Create;
                case ContractStatus.Proposed:
                    return ContractPhase.Negotiate;
                case ContractStatus.Executed:
                    return ContractPhase.Active;
                case ContractStatus.Rejected:
                case ContractStatus.Expired:
                    return ContractPhase.Obsolete;
            }

            return ContractPhase.Unspecified;
        }
    }
}
