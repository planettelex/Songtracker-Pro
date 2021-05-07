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
                }
            ];
        case UserType.LabelAdministrator:
            return [
                {
                    icon: 'mdi-file',
                    title: 'Earnings',
                    to: '/label-earnings',
                },
                {
                    icon: 'mdi-file',
                    title: 'Documents',
                    to: '/label-documents',
                },
                {
                    icon: 'mdi-file',
                    title: 'Releases',
                    to: '/label-releases',
                },
                {
                    icon: 'mdi-file',
                    title: 'Artists',
                    to: '/label-artists',
                },
                {
                    icon: 'mdi-file',
                    title: 'Users',
                    to: '/label-users',
                },
                {
                    icon: 'mdi-file',
                    title: 'Information',
                    to: '/label-information',
                }
            ];
        case UserType.PublisherAdministrator:
            return [
                {
                    icon: 'mdi-file',
                    title: 'Earnings',
                    to: '/publisher-earnings',
                },
                {
                    icon: 'mdi-file',
                    title: 'Documents',
                    to: '/publisher-documents',
                },
                {
                    icon: 'mdi-file',
                    title: 'Compositions',
                    to: '/publisher-compositions',
                },
                {
                    icon: 'mdi-file',
                    title: 'Users',
                    to: '/publisher-users',
                },
                {
                    icon: 'mdi-file',
                    title: 'Information',
                    to: '/publisher-information',
                },
            ];
        case UserType.SystemUser:
            return [
                {
                    icon: 'mdi-file',
                    title: 'Earnings',
                    to: '/my-earnings',
                },
                {
                    icon: 'mdi-file',
                    title: 'Documents',
                    to: '/my-documents',
                },
                {
                    icon: 'mdi-file',
                    title: 'Compositions',
                    to: '/my-compositions',
                },
                {
                    icon: 'mdi-file',
                    title: 'Recordings',
                    to: '/my-recordings',
                },
                {
                    icon: 'mdi-file',
                    title: 'Releases',
                    to: '/my-releases',
                },
            ];
        default:
          // no menu
      }
}
export { getSidebarMenu as default };