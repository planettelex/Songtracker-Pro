import UserType from "../../../enums/UserType";
function getSidebarMenu(userType)
{
    switch(userType) {
        case UserType.SystemAdministrator:
            return [
                {
                    icon: 'mdi-file',
                    title: 'System Information',
                    to: '/system-information',
                },
                {
                    icon: 'mdi-file',
                    title: 'Publishing Companies',
                    to: '/publishing-companies',
                },
                {
                    icon: 'mdi-file',
                    title: 'Record Labels',
                    to: '/record-labels',
                },
                {
                    icon: 'mdi-file',
                    title: 'Platforms',
                    to: '/platforms',
                },
                {
                    icon: 'mdi-file',
                    title: 'Artists',
                    to: '/artists',
                },
                {
                    icon: 'mdi-file',
                    title: 'Users',
                    to: '/users',
                },
            ];
        case UserType.LabelAdministrator:
            return [
                {
                    icon: 'mdi-file',
                    title: 'Earnings',
                    to: '/label-earnings',
                },
            ];
        case UserType.PublisherAdministrator:
            return [
                {
                    icon: 'mdi-file',
                    title: 'Earnings',
                    to: '/publisher-earnings',
                },
            ];
        case UserType.SystemUser:
            return [
                {
                    icon: 'mdi-file',
                    title: 'Earnings',
                    to: '/my-earnings',
                },
            ];
        default:
          // no menu
      }
}
export { getSidebarMenu as default };