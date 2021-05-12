import { Model as BaseModel } from 'vue-api-query';
import appConfig from "../appConfig";

export default class Model extends BaseModel {
    baseURL() {
        return appConfig.apiUrl;
      }
      request(config) {
        return this.$http.request(config);
      }
}