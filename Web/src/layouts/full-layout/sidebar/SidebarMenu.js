import UserType from "../../../enums/UserType";
function getSidebarMenu(userType)
{
    switch(userType) {
        case UserType.SystemAdministrator:
            return [
                {
                    icon: 'mdi-information',
                    title: 'System Information',
                    to: '/system-information',
                },
                {
                    icon: 'mdi-book-open-variant',
                    title: 'Publishing Companies',
                    to: '/publishing-companies',
                },
                {
                    icon: 'mdi-label-multiple',
                    title: 'Record Labels',
                    to: '/record-labels',
                },
                {
                    icon: 'mdi-cloud',
                    title: 'Platforms',
                    to: '/platforms',
                },
                {
                    icon: 'mdi-music',
                    title: 'Artists',
                    to: '/artists',
                },
                {
                    icon: 'mdi-account-multiple',
                    title: 'Users',
                    to: '/users',
                }
            ];
        case UserType.LabelAdministrator:
            return [
                {
                    icon: 'mdi-bank',
                    title: 'Earnings',
                    to: '/label-earnings',
                },
                {
                    icon: 'mdi-folder',
                    title: 'Documents',
                    to: '/label-documents',
                },
                {
                    icon: 'mdi-album',
                    title: 'Releases',
                    to: '/label-releases',
                },
                {
                    icon: 'mdi-music',
                    title: 'Artists',
                    to: '/label-artists',
                },
                {
                    icon: 'mdi-account-multiple',
                    title: 'Users',
                    to: '/label-users',
                },
                {
                    icon: 'mdi-information-variant',
                    title: 'Information',
                    to: '/label-information',
                }
            ];
        case UserType.PublisherAdministrator:
            return [
                {
                    icon: 'mdi-bank',
                    title: 'Earnings',
                    to: '/publisher-earnings',
                },
                {
                    icon: 'mdi-folder',
                    title: 'Documents',
                    to: '/publisher-documents',
                },
                {
                    icon: 'mdi-music-box-multiple',
                    title: 'Compositions',
                    to: '/publisher-compositions',
                },
                {
                    icon: 'mdi-account-multiple',
                    title: 'Users',
                    to: '/publisher-users',
                },
                {
                    icon: 'mdi-information-variant',
                    title: 'Information',
                    to: '/publisher-information',
                },
            ];
        case UserType.SystemUser:
            return [
                {
                    icon: 'mdi-bank',
                    title: 'Earnings',
                    to: '/my-earnings',
                },
                {
                    icon: 'mdi-folder',
                    title: 'Documents',
                    to: '/my-documents',
                },
                {
                    icon: 'mdi-music-box-multiple',
                    title: 'Compositions',
                    to: '/my-compositions',
                },
                {
                    icon: 'mdi-microphone',
                    title: 'Recordings',
                    to: '/my-recordings',
                },
                {
                    icon: 'mdi-album',
                    title: 'Releases',
                    to: '/my-releases',
                },
            ];
        default:
          // no menu
      }
}
export { getSidebarMenu as default };