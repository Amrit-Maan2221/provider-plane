export const setupInterceptors = (store, clients) => {
  clients.forEach((client) => {
    client.interceptors.request.use((config) => {
      const token = store.getState().auth?.accessToken;

      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }

      return config;
    });

    client.interceptors.response.use(
      (response) => response.data,
      (error) => {
        if (error.response?.status === 401) {
          store.dispatch({ type: "auth/logout" });
        }
        return Promise.reject(error);
      }
    );
  });
};
