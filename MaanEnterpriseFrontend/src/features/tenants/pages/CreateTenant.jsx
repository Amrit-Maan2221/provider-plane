import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Plus, Trash2, User, Globe, Clock } from "lucide-react";
import { createTenant } from "../tenant.api";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Checkbox } from "@/components/ui/checkbox";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Separator } from "@/components/ui/separator";
import { useTopbar } from "@/layouts/TopbarContext";

export default function CreateTenantPage() {
  const navigate = useNavigate();
  const { setActions } = useTopbar();
  
  const [form, setForm] = useState({
    name: "",
    slug: "",
    country: "",
    timezone: "",
    contacts: [{ name: "", email: "", phone: "", isPrimary: true }]
  });
  const [submitting, setSubmitting] = useState(false);

  // Sync actions with Topbar (Save button in the header)
  useEffect(() => {
    setActions(
      <div className="flex items-center gap-2">
        <Button variant="ghost" onClick={() => navigate("/tenants")} disabled={submitting}>
          Cancel
        </Button>
        <Button onClick={handleSubmit} disabled={submitting}>
          {submitting ? "Saving..." : "Create Tenant"}
        </Button>
      </div>
    );
    return () => setActions(null);
  }, [form, submitting]);

  const handleSubmit = async () => {
    setSubmitting(true);
    try {
      await createTenant(form);
      navigate("/tenants");
    } catch (err) {
      console.error(err);
      alert("Error creating tenant. Check console for details.");
    } finally {
      setSubmitting(false);
    }
  };

  // Helper to sync Name -> Slug
  const handleNameChange = (val) => {
    const slug = val.toLowerCase().replace(/\s+/g, '-').replace(/[^a-z0-t-]/g, '');
    setForm({ ...form, name: val, slug });
  };

  const addContact = () => {
    setForm({
      ...form,
      contacts: [...form.contacts, { name: "", email: "", phone: "", isPrimary: false }]
    });
  };

  const updateContact = (index, field, value) => {
    const newContacts = [...form.contacts];
    if (field === "isPrimary" && value === true) {
      // Ensure only one primary contact exists
      newContacts.forEach((c, i) => (c.isPrimary = i === index));
    } else {
      newContacts[index] = { ...newContacts[index], [field]: value };
    }
    setForm({ ...form, contacts: newContacts });
  };

  const removeContact = (index) => {
    setForm({
      ...form,
      contacts: form.contacts.filter((_, i) => i !== index)
    });
  };

  return (
    <div className="space-y-8">
      {/* SECTION 1: Organization Details */}
      <Card className="border-none shadow-none bg-transparent md:bg-card md:border md:shadow-sm">
        <CardHeader>
          <CardTitle>Organization Details</CardTitle>
          <CardDescription>The core identity of your new tenant.</CardDescription>
        </CardHeader>
        <CardContent className="grid gap-6">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="space-y-2">
              <Label htmlFor="name">Organization Name</Label>
              <Input 
                id="name" 
                placeholder="e.g. Pied Piper"
                value={form.name} 
                onChange={(e) => handleNameChange(e.target.value)} 
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="slug">URL Slug</Label>
              <div className="relative">
                <span className="absolute left-3 top-2.5 text-muted-foreground text-sm">/</span>
                <Input 
                  id="slug" 
                  className="pl-6"
                  placeholder="pied-piper"
                  value={form.slug} 
                  onChange={(e) => setForm({ ...form, slug: e.target.value })} 
                />
              </div>
            </div>
            <div className="space-y-2">
              <Label htmlFor="country" className="flex items-center gap-2">
                <Globe className="h-3.5 w-3.5" /> Country
              </Label>
              <Input 
                id="country" 
                placeholder="United States"
                value={form.country} 
                onChange={(e) => setForm({ ...form, country: e.target.value })} 
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="timezone" className="flex items-center gap-2">
                <Clock className="h-3.5 w-3.5" /> Timezone
              </Label>
              <Input 
                id="timezone" 
                placeholder="America/New_York"
                value={form.timezone} 
                onChange={(e) => setForm({ ...form, timezone: e.target.value })} 
              />
            </div>
          </div>
        </CardContent>
      </Card>

      {/* SECTION 2: Contact Persons */}
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0">
          <div>
            <CardTitle>Contact Persons</CardTitle>
            <CardDescription>Main points of contact for this account.</CardDescription>
          </div>
          <Button variant="outline" size="sm" onClick={addContact} className="h-8 gap-1">
            <Plus className="h-4 w-4" /> Add
          </Button>
        </CardHeader>
        <CardContent className="space-y-8">
          {form.contacts.map((contact, index) => (
            <div key={index} className="space-y-4">
              {index > 0 && <Separator className="my-4" />}
              
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-2 text-sm font-semibold">
                  <div className="bg-primary/10 text-primary rounded-full p-1">
                    <User className="h-4 w-4" />
                  </div>
                  Contact #{index + 1}
                </div>
                {form.contacts.length > 1 && (
                  <Button 
                    variant="ghost" 
                    size="icon" 
                    onClick={() => removeContact(index)} 
                    className="text-muted-foreground hover:text-destructive transition-colors"
                  >
                    <Trash2 className="h-4 w-4" />
                  </Button>
                )}
              </div>

              <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                <div className="space-y-2">
                  <Label className="text-xs uppercase tracking-wider text-muted-foreground">Full Name</Label>
                  <Input 
                    placeholder="John Doe"
                    value={contact.name} 
                    onChange={(e) => updateContact(index, "name", e.target.value)} 
                  />
                </div>
                <div className="space-y-2">
                  <Label className="text-xs uppercase tracking-wider text-muted-foreground">Email</Label>
                  <Input 
                    type="email" 
                    placeholder="john@example.com"
                    value={contact.email} 
                    onChange={(e) => updateContact(index, "email", e.target.value)} 
                  />
                </div>
                <div className="space-y-2">
                  <Label className="text-xs uppercase tracking-wider text-muted-foreground">Phone</Label>
                  <Input 
                    placeholder="+1 (555) 000-0000"
                    value={contact.phone} 
                    onChange={(e) => updateContact(index, "phone", e.target.value)} 
                  />
                </div>
              </div>

              <div className="flex items-center space-x-2 bg-muted/30 p-3 rounded-md w-fit">
                <Checkbox 
                  id={`primary-${index}`} 
                  checked={contact.isPrimary} 
                  onCheckedChange={(checked) => updateContact(index, "isPrimary", checked)} 
                />
                <label 
                  htmlFor={`primary-${index}`} 
                  className="text-sm font-medium cursor-pointer"
                >
                  Primary Contact
                </label>
              </div>
            </div>
          ))}
        </CardContent>
      </Card>
    </div>
  );
}