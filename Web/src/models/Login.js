import Model from './Model';

export default class User extends Model {
    resource() {
        return 'login';
      }
}