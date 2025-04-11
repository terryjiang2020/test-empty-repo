import axios from 'axios';

const apiClient = axios.create({
  baseURL: '/api',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  }
});

// Add request interceptor for authentication
apiClient.interceptors.request.use(
  config => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
  },
  error => Promise.reject(error)
);

// Dashboard data
export const fetchDashboardData = async () => {
  try {
    const [statsResponse, projectsResponse] = await Promise.all([
      apiClient.get('/stats'),
      apiClient.get('/projects')
    ]);
    
    return {
      stats: statsResponse.data,
      items: projectsResponse.data
    };
  } catch (error) {
    console.error('API error:', error);
    throw error;
  }
};

// Projects
export const fetchProjects = () => {
  return apiClient.get('/projects');
};

export const fetchProject = (id) => {
  return apiClient.get(`/projects/${id}`);
};

export const createProject = (project) => {
  return apiClient.post('/projects', project);
};

export const updateProject = (id, project) => {
  return apiClient.put(`/projects/${id}`, project);
};

export const deleteProject = (id) => {
  return apiClient.delete(`/projects/${id}`);
};

// Authentication
export const login = (credentials) => {
  return apiClient.post('/auth/login', credentials);
};

export const register = (userData) => {
  return apiClient.post('/auth/register', userData);
};

export const logout = () => {
  localStorage.removeItem('token');
  return Promise.resolve();
};

// Task Report API calls
export const generateTaskReport = async (reportParams) => {
  return apiClient.post('/api/TaskReport/generate', reportParams);
};

export const getTaskReport = async (reportId) => {
  return apiClient.get(`/api/TaskReport/${reportId}`);
};