namespace _00_Framework.Application
{
    /// <summary>
    /// An enum to help the status of relation request between 2 Users
    /// </summary>
    public enum RequestStatus
    {
        WithoutRequest = 0,
        RequestPending = 1,
        RequestAccepted=2,
        RevertRequestPending=3,
        RevertRequestAccepted=4,
        ErrorWithRelationNumbers=5,
        UnknownError=6
    }
}