namespace DaiPhucVinh.Shared.Core
{
    public enum CommonErrorStatus
    {
        /// <summary>
        /// Vd khong thay Id can tim
        /// </summary>
        KeyNotFound = 0,
        /// <summary>
        /// Vd ten, email, Id trung
        /// </summary>
        DuplicateData = 1,
        /// <summary>
        /// Vd Id cua metadata do client truyen len khong khop voi Id cua metadata tren Server
        /// </summary>
        MetaNotMatch = 2,
        /// <summary>
        /// khong co day du thong tin can thiet de create/update data
        /// </summary>
        MissingRequiredData = 3,
        BadOperation = 4,
    }
}


