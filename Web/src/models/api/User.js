import Model from './Model';
import UserAccount from './UserAccount';

export default class User extends Model {
    resource() {
        return 'users';
      }

      accounts() {
        return this.hasMany(UserAccount);
      }

      relations() {
        return {
          'accounts': UserAccount,
        }
      }
}