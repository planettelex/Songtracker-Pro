import UserType from '../../enums/UserType';

const UserTypes = [
    { key: "SystemUser", name: null, value: UserType.SystemUser },
    { key: "PublisherAdministrator", name: null, value: UserType.PublisherAdministrator },
    { key: "RecordLabelAdministrator", name: null, value: UserType.LabelAdministrator },
    { key: "SystemAdministrator", name: null, value: UserType.SystemAdministrator }
]

export default UserTypes;