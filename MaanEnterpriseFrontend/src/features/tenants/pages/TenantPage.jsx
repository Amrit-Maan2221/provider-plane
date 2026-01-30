import { useEffect, useState } from "react";
import { getTenantsPaged } from "../tenant.api";
import { useTopbar } from "@/layouts/TopbarContext";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Link } from "react-router-dom";
import { Button } from "@/components/ui/button";



export default function TenantPage() {
  const [tenants, setTenants] = useState([]);
  const [loading, setLoading] = useState(false);
  const { setActions } = useTopbar();

  const loadTenants = async () => {
    setLoading(true);
    try {
      const res = await getTenantsPaged(1, 10);
      setTenants(res.data?.items ?? res.data);
    } finally {
      setLoading(false);
    }
  };

   useEffect(() => {
    // Instead of dialog, show a link/button
    setActions(
    <Button asChild variant="default">
        <Link to="/tenants/create">Create Tenant</Link>
    </Button>
    );
    return () => setActions(null);
}, []);

    useEffect(() => {
        loadTenants();
    }, []);

  return (
    <div>
        <Table>
        <TableHeader>
            <TableRow>
            <TableHead>Name</TableHead>
            <TableHead>Slug</TableHead>
            <TableHead>Country</TableHead>
            <TableHead>Timezone</TableHead>
            </TableRow>
        </TableHeader>

        <TableBody>
            {tenants.map((tenant) => (
            <TableRow key={tenant.id}>
                <TableCell>{tenant.name}</TableCell>
                <TableCell>{tenant.slug}</TableCell>
                <TableCell>{tenant.country}</TableCell>
                <TableCell>{tenant.timezone}</TableCell>
            </TableRow>
            ))}
        </TableBody>
        </Table>
    </div>
  );
}
